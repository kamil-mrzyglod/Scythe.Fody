using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Scythe.Fody.Scythes
{
    public class Boxing : IScythe
    {
        private static readonly OpCode[] LoopInstructions =
        {
            OpCodes.Ldc_I4, OpCodes.Cgt, OpCodes.Ldc_I4, OpCodes.Ceq,
            OpCodes.Stloc, OpCodes.Ldloc, OpCodes.Brtrue
        };

        /// <summary>
        /// Checks whether any method performs a boxing
        /// operation inside a loop. It doesn't count 
        /// single boxing operations and doesn't check
        /// if a loop calls a method which internally 
        /// performs a boxing(and this is an enhancement,
        /// which should be developed soon.
        /// </summary>
        public IEnumerable<ErrorMessage> Check(MethodDefinition definition, XElement config)
        {
            var element = config.Elements("Boxing").FirstOrDefault();
            if (element == null)
                yield break;

            var boxingInstructions = definition.Body.Instructions.Where(_ => _.OpCode == OpCodes.Box).ToArray();
            if(boxingInstructions.Any() == false)
                yield break;

            var severity = element.Attribute("Severity").Value;
            var loopSuspects =
                definition.Body.Instructions.Where(
                    LoadsVariableOntoTheEvaluationStack()).ToArray();

            if (loopSuspects.Any() == false)
                yield break;

            foreach (var suspect in loopSuspects)
            {
                Instruction ret;
                if (!TryToParseAsLoop(suspect, 0, out ret)) continue;

                foreach (var instruction in boxingInstructions.Where(instruction => instruction.Offset > ((Instruction) ret.Operand).Offset))
                {
                    yield return
                        new ErrorMessage(
                            $"Method {definition.FullName} performs boxing inside a loop at offset {instruction.Offset}, what can hurt performance.",
                            ErrorType.Boxing, severity);
                }
            }
        }

        private static Func<Instruction, bool> LoadsVariableOntoTheEvaluationStack()
        {
            return _ =>
                _.OpCode == OpCodes.Ldloc || _.OpCode == OpCodes.Ldloc_0 || _.OpCode == OpCodes.Ldloc_1 ||
                _.OpCode == OpCodes.Ldloc_2 || _.OpCode == OpCodes.Ldloc_3;
        }

        private static bool TryToParseAsLoop(Instruction suspect, int currentIndex, out Instruction ret)
        {
            while (true)
            {
                if (currentIndex >= LoopInstructions.Length)
                {
                    ret = suspect;
                    return true;
                }

                var instruction = LoopInstructions[currentIndex];
                if (LoopOpCodesAreEqual(suspect.Next.OpCode, instruction) == false)
                {
                    ret = null;
                    return false;
                }

                suspect = suspect.Next;
                currentIndex++;
            }
        }

        private static bool LoopOpCodesAreEqual(OpCode first, OpCode second)
        {
            OpCode[] validCodes;
            switch (second.Code)
            {
                case Code.Ldc_I4:
                    validCodes = new[]
                    {
                        OpCodes.Ldc_I4, OpCodes.Ldc_I4_0, OpCodes.Ldc_I4_1, OpCodes.Ldc_I4_2,
                        OpCodes.Ldc_I4_3, OpCodes.Ldc_I4_4, OpCodes.Ldc_I4_5, OpCodes.Ldc_I4_6, OpCodes.Ldc_I4_7,
                        OpCodes.Ldc_I4_8, OpCodes.Ldc_I4_S
                    };

                    return validCodes.Contains(first);
                case Code.Stloc:
                    validCodes = new[]
                    {OpCodes.Stloc, OpCodes.Stloc_0, OpCodes.Stloc_1, OpCodes.Stloc_2, OpCodes.Stloc_3, OpCodes.Stloc_S};

                    return validCodes.Contains(first);
                case Code.Ldloc:
                    validCodes = new[]
                    {OpCodes.Ldloc, OpCodes.Ldloc_0, OpCodes.Ldloc_1, OpCodes.Ldloc_2, OpCodes.Ldloc_3, OpCodes.Ldloc_S};

                    return validCodes.Contains(first);
                case Code.Brtrue:
                    validCodes = new[]
                    {OpCodes.Brtrue, OpCodes.Brtrue_S};

                    return validCodes.Contains(first);
                case Code.Cgt:
                    return first == OpCodes.Cgt;
                case Code.Ceq:
                    return first == OpCodes.Ceq;
                default:
                    return false;
            }
        }
    }
}